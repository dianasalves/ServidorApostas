using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServidorApostas.Data;
using ServidorApostas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServidorApostas
{
    public class GreeterService : Apostas.ApostasBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly ServidorApostaContext _db;

        public GreeterService(ILogger<GreeterService> logger, ServidorApostaContext db)
        {
            _logger = logger;
            _db = db;
        }

        //Atender ao pedido ApostaRegisto do Cliente para Utilizador
        public override async Task<Resposta> ApostaRegisto(ApostaPedido pedido, ServerCallContext context)
        {
            //Verifica se já existe algum utilizador com esse nome
            var utilizador = _db.MUtilizador.FirstOrDefault(x => x.nome == pedido.Aposta.NomeUtilizador);
            if (utilizador == null) //Se não existir, cria um novo Utilizador e adiciona à base de dados
            { 
                utilizador = new ModelUtilizador { nome = pedido.Aposta.NomeUtilizador };
                _db.Add(utilizador);
            }
            //Cria uma nova aposta e adiciona à base de dados
            ModelApostas aposta = new ModelApostas
            {
                chave = pedido.Aposta.Chave,
                data = pedido.Aposta.Data,
                registada = false,
                utilizadorid = utilizador.id
            };
            _db.Add(aposta);

            //Tenta guardar as alterações na base de dados e responder ao cliente 
            try
            {
                await _db.SaveChangesAsync();
                _logger.LogInformation("Utilizador {0} chave registada: {1}", utilizador.nome, aposta.chave);
                return await Task.FromResult(new Resposta { Sucesso = true });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Erro na atualização da database - GreeterService.cs: ApostaRegisto");
                return await Task.FromResult(new Resposta { Sucesso = false });
            }
        }

        //Atender ao pedido ApostaLista dos clientes para Utilizador e Administrador
        public override async Task<ListaA> ApostaLista(ApostaListaPedido pedido, ServerCallContext context)
        {
            List<ApostaM> listaApostas = new List<ApostaM>();
            List<ModelApostas> apostasL = new List<ModelApostas>();

            // Se o nome enviado pelo cliente for vazio, quer dizer que foi enviado 
            // pelo Cliente Administrador, ou seja pede todas as apostas
            if (pedido.NomeUtilizador == "")
            {
                apostasL = await _db.MApostas.Include(b => b.utilizador)
                    .Where(x => x.registada == false && x.utilizador.nome != "Vencedora")
                    .OrderByDescending(x => x.data).ToListAsync();
            }
            else
            {   // Caso contrário procura o nome na base de dados e devolve uma lista
                // com as apostas desse User
                var utilizador = await _db.MUtilizador.Where(x => x.nome == pedido.NomeUtilizador).FirstOrDefaultAsync();
                if (utilizador != null)
                {
                    apostasL = await _db.MApostas.Include(b => b.utilizador)
                        .Where(x => x.utilizadorid == utilizador.id)
                        .OrderByDescending(x => x.data).ToListAsync();
                }
            }
            foreach (var a in apostasL)
            {
                var aposta = new ApostaM { NomeUtilizador = a.utilizador.nome, Chave = a.chave, Data = a.data };
                listaApostas.Add(aposta);
            }


            _logger.LogInformation("Admintrador pediu a lista de apostas. {0} apostas foram retornadas.", apostasL.Count());
            return await Task.FromResult(new ListaA { Aposta = { listaApostas } });
        }

        // Atender ao pedido Listar Utilizadores do Cliente Administrador
        public override async Task<ListaU> UtilizadorLista(ListaUtilizadorPedido pedido, ServerCallContext context)
        {
            ListaU utilizadorLista = new ListaU();

            // guardar numa lista utilizadores cujo nome não é "Vencedora" (Aposta vencedora)
            // e cujas apostas não estejam arquivadas
            var utilizadores = await _db.MApostas.Include(b => b.utilizador)
                .Where(x => x.registada == false && x.utilizador.nome != "Vencedora")
                .Select(a => a.utilizador.nome).Distinct().ToListAsync();

            foreach (var u in utilizadores)
            {
                utilizadorLista.Utilizadores.Add(u);
            }

            _logger.LogInformation("Administrador pediu os utilizadores registados no sistema. {0} utilizadores retornados.", utilizadores.Count());
            return await Task.FromResult(utilizadorLista);
        }

        // Atender ao pedido Arquivar Apostas do Cliente Administrador
        public override async Task<Resposta> ApostaGuadar(PedidoGuardar pedido, ServerCallContext context)
        {
            // Procurar todas as apostas que não estão arquivadas e alterar a coluna Arquivada
            var apostasADecorrer = _db.MApostas.Where(x => x.registada == false).ToList();
            foreach (var apostas in apostasADecorrer)
            {
                apostas.registada = true;
            }

            try
            {
                await _db.SaveChangesAsync();
                _logger.LogInformation("Admintrador pediu para guardar as apostas. {0} apostas foram guardadas.", apostasADecorrer.Count());
                return await Task.FromResult(new Resposta { Sucesso = true });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Erro na atualização da database -> GreeterService.cs: ApostaGuadar");
                return await Task.FromResult(new Resposta { Sucesso = false });
            }
        }

        // Atender ao pedido de Registo de Chave Vencedora do Cliente Gestor
        public override async Task<Resposta> ChaveWinRegisto(ChaveWin pedido, ServerCallContext context)
        {
            // Verificar se já existe alguma chave Vencedora que não esteja arquivada
            var chaveWin = await _db.MApostas.Include(x => x.utilizador)
                .Where(b => b.registada == false)
                .AnyAsync(u => u.utilizador.nome == "Vencedora");
            if (chaveWin)   // Se já existir, não deixar inserir uma nova
            {
                _logger.LogWarning("Gestor tentou registar a chave vencedora, mas já existe uma chave vencedora não registada.");
                return await Task.FromResult(new Resposta { Sucesso = false });
            }

            // Verificar se já existe o User "Vencedora", onde serão associadas as chaves vencedoras
            // Se ainda não existir cria-lo e adicionar à base de dados
            var ChaveWinU = await _db.MUtilizador.FirstOrDefaultAsync(u => u.nome == "Vencedora");
            if (ChaveWinU == null)
            {
                ChaveWinU = new ModelUtilizador { nome = "Vencedora" };
                _db.Add(ChaveWinU);
            }

            // Adicionar chave vencedora à base de dados
            var apostaWin = new ModelApostas
            {
                chave = pedido.ChaveVencedora,
                data = DateTime.Now.ToString(),
                registada = false,
                utilizador = ChaveWinU
            };
            _db.Add(apostaWin);

            try
            {
                await _db.SaveChangesAsync();
                _logger.LogInformation("Gestor registou a aposta vencedora: {0}.", apostaWin.chave);
                return await Task.FromResult(new Resposta { Sucesso = true });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Erro na atualização da database -> GreeterService.cs: ChaveWinRegisto");
                return await Task.FromResult(new Resposta { Sucesso = false });
            }
        }

        // Atender ao pedido de Ver as Apostas Vencedoras do Cliente Gestor
        public override async Task<ListaAWin> ApostaWinLista(ApostaWinPedido pedido, ServerCallContext context)
        {
            // Ver qual é a aposta Vencedora Ativa (User == "Vencedora" e Arquivada == false)
            var apostaWin = await _db.MApostas.Include(x => x.utilizador)
                .Where(b => b.utilizador.nome == "Vencedora")
                .FirstOrDefaultAsync(b => b.registada == false);

            List<ModelApostas> aposta = new List<ModelApostas>();

            // Se existir aposta vencedora, procurar na base de dados todas as
            // apostas em que a chave é igual à da chave vencedora
            if (apostaWin != null)
            {
                aposta = await _db.MApostas.Include(x => x.utilizador)
                        .Where(a => a.registada == false
                            && a.utilizador.nome != "Vencedora"
                            && a.chave == apostaWin.chave)
                                .OrderByDescending(x => x.data).ToListAsync();
            }

            List<ApostaM> apostasL = new List<ApostaM>();
            foreach (var aL in aposta)
            {
                apostasL.Add(new ApostaM { Chave = aL.chave, Data = aL.data, NomeUtilizador = aL.utilizador.nome });
            }

            _logger.LogInformation("Gestor requested winning keys. {0} keys were returned.", apostasL.Count());
            return await Task.FromResult(new ListaAWin { ApostaVencedora = { apostasL } });
        }
    }
}
