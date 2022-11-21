using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Extensions.CognitoAuthentication;
using MyBox;
using System.Runtime.CompilerServices;

public class CognitoManager : MonoBehaviour
{
  
    [ReadOnly]public  string jwt;
    #region Credentials
    public const string identityPool = "us-east-1:18f13f7d-847c-4ceb-835d-294fdb012630";
    public static string userPoolId = "us-east-1_CwvwPCoju";
    public static string appClientId = "4ce8h6mnh708s5psft6omkneuk";

    public static RegionEndpoint region = RegionEndpoint.USEast1;

    public static CognitoAWSCredentials credentials = new CognitoAWSCredentials(
        identityPool, region
    );

    AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient (new Amazon.Runtime.AnonymousAWSCredentials(), region);

    #endregion

    #region SignUp
    async void SignUp(string email, string password,string Username)
    {
        SignUpRequest signUpRequest = new SignUpRequest()
        {
            ClientId = appClientId,
            Username = email,
            Password = password,
        };

        List<AttributeType> attributes = new List<AttributeType>()
        {
            new AttributeType(){Name = "email", Value = email}
          //  new AttributeType(){Name = "preferred username", Value = Username}
        };

        signUpRequest.UserAttributes = attributes;

        try
        {
            SignUpResponse request = await provider.SignUpAsync(signUpRequest);
            Debug.Log("Sign up ");
           
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
            return;
        }
    }
    #endregion

    #region Logout
    async void Logout()
    {
        GlobalSignOutRequest LogoutReq = new GlobalSignOutRequest()
        {
            AccessToken = jwt
        };
        try
        {
            GlobalSignOutResponse LogOut = await provider.GlobalSignOutAsync(LogoutReq);
            Debug.Log("Logged Out");
            jwt = null;
        }
        catch(Exception e)
        {
            Debug.Log("Exception: " + e);
            return;
        }
    }
    #endregion

    #region Login 
    async void Login(string userName,string password)
    {
        CognitoUserPool userPool = new CognitoUserPool(userPoolId, appClientId, provider);
        CognitoUser user = new CognitoUser(userName, appClientId, userPool, provider);

        InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
        {
            Password = password
        };

        try
        {
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            GetUserRequest getUserRequest = new GetUserRequest();
            getUserRequest.AccessToken = authResponse.AuthenticationResult.AccessToken;

            jwt = getUserRequest.AccessToken;

            Debug.Log("Logged In");
          //  Debug.Log(jwt);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
            return;
        }

       
    }
    #endregion

    #region Testing
    [Foldout("SignUptesing",true)]
    public string email;
    public string pw;
    public string user;

    [ButtonMethod]
    void _SignUp()
    {
        SignUp(email, pw,user);    
    }

    [Foldout("Logintesting", true)]
    public string LoginID;
    public string _pw;

    [ButtonMethod]
    void _Login()
    {
        Login(LoginID, _pw);
    }

    [ButtonMethod]
    void _LogOut()
    {
        Logout();
    }
    #endregion
}
