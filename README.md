# Teste CORS

Encontrei um problema em um servidor Windows 2012 R2 onde nossa aplicação padrão não estava conseguindo trabalhar com CORS, como é um servidor de cliente não foi feito pelo nosso time. Ele não respondia de forma alguma requisições preflight (OPTIONS) do nosso angular. Como trabalhamos com multitenant temos diferentes dominios na mesma aplicação.

Após várias investidas para analisar o problema, resolvi fazer uma página em javascript usando requisições *[xmlhttprequest](https://developer.mozilla.org/pt-BR/docs/Web/API/XMLHttpRequest)* e *[fetch](https://developer.mozilla.org/pt-BR/docs/Web/API/Fetch_API/Using_Fetch)*, para testar na minha aplicação, e mesmo assim continuava o erro.
>***Dica**: importante é não fazer um request comum pois ele não ira chamar o preflight, adicionado um header customizado ele já vai fazer a requisição.* 

Resolvi fazer uma aplicação backend ([WebAPI 2](https://learn.microsoft.com/pt-br/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api)) para testar se poderia ser algo no nosso backend, mas também não resolveu o problema, tudo funcionando normalmente no meu ambiente e no servidor do cliente nada!

Montamos um novo servidor com as mesmas configurações e tudo funcionando perfeitamente, analisando com calma o servidor me deparei com eventos chegando no IIS e não passava para o servidor da aplicação! Validando regras do IIS identifiquei um bloqueio no **[Request Filtering](https://idreesdotnet.blogspot.com/2022/05/configure-iis-for-cors-preflight.html)** para o Verbo OPTIONS no servidor, afetando todos os sites configurados. Após várias analisas uma simples configuração estava bloqueando!  Pelo menos serviu como fonte de estudo, o código no repositório tem a finalide de teste o back e front para testar o CORS :)

# Como funciona o CORS

O CORS *(Compartilhamento de Recursos entre Origens)* é um padrão W3C que permite que um servidor relaxe a política de mesma origem. Usando o CORS, um servidor pode explicitamente permitir algumas solicitações entre origens e rejeitar outras. Ele trabalha nos Headers do HTTP, com isso o Browser consegue identificar as regras para utilizar.

### Request do Browser

O Browser com suporte a CORS deve enviar o header **Origin**, para o servidor conseguir responder corretamente o Request.

    OPTIONS http://myservice.azurewebsites.net/api/test HTTP/1.1
    Accept: */*
    Origin: http://myclient.azurewebsites.net
    Access-Control-Request-Method: PUT
    Access-Control-Request-Headers: accept, x-my-custom-header
    Accept-Encoding: gzip, deflate
    User-Agent: Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)
    Host: myservice.azurewebsites.net
    Content-Length: 0
    
### Response do Servidor

O servidor irá responder com o header(s) **Access-Control-Allow-*****.
**Access-Control-Allow-Origin**: O valor desse cabeçalho corresponde ao cabeçalho *Origin* ou é o valor *curinga* "*"
**Access-Control-Request-Method**: o método HTTP que será usado para a solicitação real.
**Access-Control-Request-Headers**: uma lista de cabeçalhos de solicitação que o aplicativo definiu na solicitação real. 

    HTTP/1.1 200 OK
    Cache-Control: no-cache
    Pragma: no-cache
    Content-Length: 0
    Access-Control-Allow-Origin: http://myclient.azurewebsites.net
    Access-Control-Allow-Headers: x-my-custom-header
    Access-Control-Allow-Methods: PUT
    Date: Wed, 05 Jun 2013 06:33:22 GMT

# Configurações do IIS
Para configurar o IIS para permitir que um aplicativo ASP.NET receba e manipule solicitações OPTION, adicione a seguinte configuração ao arquivo **web.config** do aplicativo na *<system.webServer><handlers>* seção:

    <system.webServer>
      <handlers>
        <remove name="ExtensionlessUrlHandler-Integrated-4.0" />
        <remove name="OPTIONSVerbHandler" />
        <add name="ExtensionlessUrlHandler-Integrated-4.0" path="*." verb="*" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />
      </handlers>
    </system.webServer>

# Nosso problema   
Infelizmente

## Referencias

- https://learn.microsoft.com/pt-br/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api
- https://idreesdotnet.blogspot.com/2022/05/configure-iis-for-cors-preflight.html
- https://stackoverflow.com/questions/22495240/iis-hijacks-cors-preflight-options-request
- https://stackoverflow.com/questions/49450854/how-to-authorize-cors-preflight-request-on-iis-with-windows-authentication

 