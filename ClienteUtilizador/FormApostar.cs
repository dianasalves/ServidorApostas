using Grpc.Core;
using Grpc.Net.Client;
using ServidorApostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClienteUtilizador
{
    public partial class FormApostar : Form
    {
        private string nomeutilizador;
        private GrpcChannel channel;
        public FormApostar(string _nome, GrpcChannel _channel)
        {
            InitializeComponent();
            nomeutilizador = _nome;
            channel = _channel;

            try
            {
                // enviar nome do utilizador para o servidor
                // no pedido de lista de apostas
                var cliente = new Apostas.ApostasClient(channel);
                var resposta = cliente.ApostaLista(new ApostaListaPedido{ NomeUtilizador = nomeutilizador });

                // Apresentar a lista de apostas do utilizador
                foreach (var apostaslistadas in resposta.Aposta)
                {
                    listView1.Items.Add(new ListViewItem(new[] { apostaslistadas.NomeUtilizador, apostaslistadas.Chave, apostaslistadas.Data }));
                }
            }
            catch (RpcException)
            {
                MessageBox.Show("Erro no serviço. Por favor tente mais tarde.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string chave = ordenar();
            if(chave == "-1")
            {
                MessageBox.Show("Por favor verifique a chave. Erro foi encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ApostaM apostaM = new ApostaM { NomeUtilizador = nomeutilizador, Chave = chave, Data = DateTime.Now.ToString() };
                ApostaPedido aposta_pedido = new ApostaPedido { Aposta = apostaM };

                try
                {
                    var client = new Apostas.ApostasClient(channel);
                    var reply = client.ApostaRegisto(aposta_pedido);
                }
                catch (RpcException)
                {
                    MessageBox.Show("Erro no serviço. Por favor tente mais tarde.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ordenar()
        {
            int[] numeros = new int[5];
            int[] estrelas = new int[2];
            int i = 0;

            List<TextBox> textBoxesNumeros = new List<TextBox> {
                textBoxNumero1,
                textBoxNumero2,
                textBoxNumero3,
                textBoxNumero4,
                textBoxNumero5};

            List<TextBox> textBoxesEstrelas = new List<TextBox> {
                textBoxEstrela1,
                textBoxEstrela2};

            // Converter para INT os números inseridos pelo utilizador (string)
            foreach (var t in textBoxesNumeros)
            {
                if (!Int32.TryParse(t.Text, out numeros[i++]))
                    return "-1";
            }
            i = 0;
            foreach (var t in textBoxesEstrelas)
            {
                if (!Int32.TryParse(t.Text, out estrelas[i++]))
                    return "-1";
            }

            // Verificar se números estão entre 1 e 50, e estrelas entre 1 e 12
            if (numeros.Max() > 50 || numeros.Min() < 1 || estrelas.Max() > 12 || estrelas.Min() < 1)
            {
                return "-1";
            }

            // Ordenar números e estrelas
            Array.Sort(numeros);
            Array.Sort(estrelas);

            // Verificar se números es estrelas são diferentes
            for (i = 0; i < numeros.Length - 1; i++)
            {
                if (numeros[i] == numeros[i + 1])
                    return "-1";
            }
            if (estrelas[0] == estrelas[1])
                return "-1";

            // concatenar números e estrelas numa única string para enviar para o servidor
            string chaveOrdenada = string.Join(" ", numeros);
            string estrelasOrdenada = string.Join(" ", estrelas);
            string chave = chaveOrdenada + " + " + estrelasOrdenada;
            return chave;
        }
    }
}
