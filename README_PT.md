# ServidorApostas
  No concurso Euromilhões, as chaves são compostas por: cinco números diferentes, no intervalo de 1 a 50; e duas estrelas, que correspondem a dois números diferentes, no 
intervalo de 1 a 12. Com este projeto pretendeu-se criar um sistema cliente/servidor que implemente um serviço de sugestão de chaves. Foi implementado um programa 
cliente e um programa servidor usando a linguagem C. Foi também especificado um protocolo de comunicação entre o cliente/servidor que obtenção de uma ou várias chaves.

Servidor:
    O servidor recebe e responde a vários pedidos simultaneamente usando um modelo de dispachment com threads ou listen; 
        Quando inicialmente contactado, o servidor responde com uma mensagem de “100 OK”;
        Se o servidor receber uma mensagem “QUIT”, responde com a mensagem “400 BYE” e termina a comunicação com o cliente.
Cliente:
    O cliente deve receber como parâmetro ou pedir ao utilizador o endereço IP do servidor a contactar. Liga-se ao servidor e permite ao utilizador solicitar uma ou várias chaves, 
    implementando o protocolo definido e uma interface de texto simples. 
Protocolo:
    O protocolo deve incluir as mensagens e os estados necessários para o diálogo cilente/servidor necessário para implementar o serviço.
Implementação:
    A implementação deve ser efetuada usando winsocks 2.2 em Visual Studio C.

Lista de comandos:
    •	“CHAVE / Chave / chave + numero de chaves a criar” – Gera o número pedido de chaves do Euromilhões;
    •	“QUIT/ Quit / quit” – Encerra a comunicação com o servidor.
Nota: Foi implementado de forma que aceite quer letras maiúsculas quer letras minúsculas.

Mensagens/Respostas possíveis:
  •	“100 OK” – Conectado
  •	“200 Sent” – Recebido com sucesso
  •	“400 Bye” – Conexão terminada
  •	“404 NaoEncontrado” – Número de chaves inválido
  •	“410 Perdido” - Está em falta o número de chaves para serem criadas
  •	“450 MalDirecionado” – Comando desconhecido
  
Estados:
  •	A aguardar pedido
  •	À espera de conexões
  •	Conectado
  •	Conexão falhada
  •	Desconectado
  •	Erro no pedido
