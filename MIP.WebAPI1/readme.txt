Scenario: Protected web API

Here is specific information you need to know to protect web APIs:

Your app registration must expose at least one scope or one application role.
 a. Scopes are exposed by web APIs that are called on behalf of a user. To client applications, scopes show up as delegated permissions 
 and app roles show up as application permissions for your web API.
 b. Application roles are exposed by web APIs called by daemon applications (that calls your web API on their own behalf).

 Scopes also appear on the consent window that's presented to users of your app. Therefore, provide the corresponding
 strings that describe the scope:

   a. As seen by a user.
   b. As seen by a tenant admin, who can grant admin consent.

If your web API is called by a daemon app
In this section, you learn how to register your protected web API so that daemon apps can securely call it.

You declare and expose only application permissions because daemon apps don't interact with users. Delegated permissions wouldn't make sense.
Tenant admins can require Azure AD to issue web API tokens only to applications that have registered to access one of the API's application permissions
App roles cannot be consented to by a user (as they're used by an application that call the web API on behalf of itself). 
A tenant administrator will need to consent to client applications of your web API exposing app roles. See Admin consent for details.
https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-app-registration#application-id-uri-and-scopes
  
 App Registration
 
 1. Find the property accessTokenAcceptedVersion in the manifest. = 2

No redirect URI - Web APIs don't need to register a redirect URI because no user is interactively signed in.

If you want Azure AD to allow access to your web API from only certain clients, set User assignment required? to Yes.

If you set User assignment required? to Yes, Azure AD checks the app role assignments of a client when it requests a web API access token.
If the client isn't assigned to any app roles, Azure AD will return the error message 
"invalid_client: AADSTS501051: Application <application name> isn't assigned to a role for the <web API>".

If you keep User assignment required? set to No, Azure AD won’t check the app role assignments when a client requests an access token 
for your web API. Any daemon client, meaning any client using the client credentials flow, 
can get an access token for the API just by specifying its audience. Any application can access the API without having to request permissions for it.

The scope to request for a client credential flow is the name of the resource followed by /.default. This notation tells Azure Active Directory (Azure AD) 
to use the application-level permissions declared statically during application registration.
Also, these API permissions must be granted by a tenant administrator.






-------------------------------------------------------------------------------------------------------------------------------------------------
Scenario: A web app that authenticates users and calls web APIs

Learn how to build a web app that signs users in to the Microsoft identity platform, and then calls web APIs on behalf of the signed-in user.

Web apps that call web APIs are confidential client applications. That's why they register a secret (an application password or certificate) 
with Azure Active Directory (Azure AD). This secret is passed in during the call to Azure AD to get a token.

1. This scenario covers how to call web APIs from a web app. 
You must get access tokens for those web APIs. You use MSAL libraries to acquire these tokens.

----------------------------------------------------------------------------------------------------------------------------------------------------
A web app that calls web APIs: App registration
However, because the web app now also calls web APIs, it becomes a confidential client application. That's why some extra registration is required. 
The app must share client credentials, or secrets, with the Microsoft identity platform.
  a. add a client secret (https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-web-app-call-api-app-registration)


Given that your web app now calls a downstream web API, provide a client secret or client certificate in the appsettings.json file.
You can also add a section that specifies:

The URL of the downstream web API => GraphBeta: BaseUrl
The scopes required for calling the API => GraphBeta: Scope

Startup.cs
Your web app will need to acquire a token for the downstream API. You specify it by adding the .EnableTokenAcquisitionToCallDownstreamApi() 
line after .AddMicrosoftIdentityWebApi(Configuration). This line exposes the ITokenAcquisition service that you can use in your controller and 
page actions. However, as you'll see in the following two options, it can be done more simply. 
You'll also need to choose a token cache implementation, for example .AddInMemoryTokenCaches(), in Startup.cs:


 you'll use it to acquire a token to call a web API. In ASP.NET or ASP.NET Core, calling a web API is done in the controller:

Get a token for the web API by using the token cache. 
To get this token, you call the MSAL AcquireTokenSilent method (or the equivalent in Microsoft.Identity.Web (ITokenAcquisition ).




Reference:

1. https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-web-app-call-api-overview
2. https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-overview
3. https://github.com/Azure-Samples/ms-identity-dotnet-webapi-azurefunctions
4. https://docs.microsoft.com/en-us/azure/active-directory/develop/sample-v2-code#web-applications
5. https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2