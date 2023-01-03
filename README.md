# ServerBetting

  In the Euromillions contest, the keys are made up of: five different numbers, in the range of 1 to 50; and two stars, which correspond to two different numbers, in the range of 1 to 12. The aim of this project was to create a client/server system that implements a key suggestion service. A client program and a server program were implemented using the C language. A communication protocol between the client/server that obtains one or several keys was also specified.

Server: 
    The server receives and responds to multiple requests simultaneously using a threaded dispatch or listen model; When initially contacted, the server responds with a “100 OK” message; If the server receives a “QUIT” message, it responds with a “400 BYE” message and ends communication with the client. 

Client: 
    The client must receive as a parameter or ask the user for the IP address of the server to be contacted. It connects to the server and allows the user to request one or more keys, implementing the defined protocol and a simple text interface. 

Protocol: 
    The protocol must include the messages and states necessary for the client/server dialog needed to implement the service. 
  
Implementation: 
    Implementation was done using winsocks 2.2 in Visual Studio C.

List of commands: 
  • “KEY / Key / key + number of keys to create” – Generates the requested number of Euromillions keys; 
  • “QUIT/ Quit / quit” – Terminates communication with the server. Note: It has been implemented so that it accepts both uppercase and lowercase letters.

Possible Messages/Responses: 
  • “100 OK” – Connected 
  • “200 Sent” – Successfully received 
  • “400 Bye” – Terminated connection 
  • “404 NaoEncontrado” – Invalid number of keys 
  • “410 Lost” - The number is missing of keys to be created 
  • “450 Misdirected” – Unknown command

States: 
  • Awaiting request 
  • Waiting for connections 
  • Connected 
  • Connection failed 
  • Disconnected 
  • Request error
