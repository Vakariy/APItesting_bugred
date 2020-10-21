﻿using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using TechTalk.SpecFlow;

namespace APItesting.Steps
{
    [Binding]
    public class NegativeRegisterration
    {
        RestClient client;
        Dictionary<string, string> userdata;
        Dictionary<string, string> invalidEmailData;
        Dictionary<string, object> companyData = new Dictionary<string, object>();
        Dictionary<string, object> newUserData = new Dictionary<string, object>();
        IRestResponse response;
        string userName;
        string email;
        string invalidEmail;
        string companyName;
        string companyType;       
        string emailOwner;

        [Given(@"create of a new client")]
        public void GivenCreateOfANewClient()
        {
            client = new RestClient("http://users.bugred.ru");
        }
        
        // Registrarion a new account
        [Given(@"dates like e-mail of user, name of user and password of user is ready for create a new account")]
        public void GivenDatesLikeE_MailOfUserNameOfUserAndPasswordOfUserIsReadyForCreateANewAccount()
        {
            string time = DateTime.Now.ToString();
            string temp;
            string pass;
            temp = time.Replace(":", ".").Replace(" ", "").Replace("/", "");
            userName = temp;
            email = "user" + temp + "@gmail.com";
            pass = "Qwe1234";

            userdata = new Dictionary<string, string>
            {
                {"name", userName },
                {"email", email },
                {"password", pass }
            };
        }
        
        [When(@"send request with valid datas for succesfull registration")]
        public void WhenSendRequestWithValidDatasForSuccesfullRegistration()
        {
            RestRequest request = new RestRequest("tasks/rest/doregister", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userdata);
            response = client.Execute(request);
        }
        
        [Then(@"account has been created")]
        public void ThenAccountHasBeenCreated()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }
        
        [Then(@"user name which we get from request equal user name which we sended by request registrarion")]
        public void ThenUserNameWhichWeGetFromRequestEqualUserNameWhichWeSendedByRequestRegistrarion()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(userName, json["name"]?.ToString());
        }
        
        [Then(@"e-mail of user which we get from request equal e-mail of user which we sended by request registrarion")]
        public void ThenE_MailOfUserWhichWeGetFromRequestEqualE_MailOfUserWhichWeSendedByRequestRegistrarion()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(email, json["email"]?.ToString());
        }

        // Register an account with existing email

        [Given(@"Data with existing email for registretion is ready")]
        public void GivenDataWithExistingEmailForRegistretionIsReady()
        {
            invalidEmail = "user102020201.46.24pm@gmail.com";

            invalidEmailData = new Dictionary<string, string>
            {              
                {"email", email }  
            };
        }

        [When(@"I send POST request with prepared data")]
        public void WhenISendPOSTRequestWithPreparedData()
        {
            RestRequest request = new RestRequest("tasks/rest/doregister", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(invalidEmailData);
            response = client.Execute(request);
        }

        [Then(@"Server status response OK")]
        public void ThenServerStatusResponseOK()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [Then(@"Server response type is error")]
        public void ThenServerResponseTypeIsError()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("error", json["type"]?.ToString());
        }

        [Then(@"Server response message is email already exist")]
        public void ThenServerResponseMessageIsEmailAlreadyExist()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("Параметр email является обязательным!", json["message"]?.ToString());
        }

        //Create a new company

        [Given(@"Data of company like name, type, users, email owner is ready")]
        public void GivenDataOfCompanyLikeNameTypeUsersEmailOwnerIsReady()
        {
            string time = DateTime.Now.ToString();
            string temp = time.Replace(":", ".").Replace(" ", "").Replace("/", "");
            companyName = temp + " company of I.Tereshchenko!";
            emailOwner = "werswerswerswers@gmail.com";
            companyData.Add("company_name", companyName);
            companyData.Add("company_type", "ООО");
            companyData.Add("email_owner", emailOwner);
            companyData.Add("company_users", new List<string> {"possuum@mail.ru", "testmail1@mail.ru"});
        }

        [When(@"send request with valid datas for succesfull create company")]
        public void WhenSendRequestWithValidDatasForSuccesfullCreateCompany()
        {
            RestRequest request = new RestRequest("/tasks/rest/createcompany", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(companyData);
            response = client.Execute(request);
        }

        [Then(@"company has been created")]
        public void ThenCompanyHasBeenCreated()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [Then(@"name company equal name company from request creating company")]
        public void ThenNameCompanyEqualNameCompanyFromRequestCreatingCompany()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string actualResult = json["company"]["name"].ToString();
            Assert.AreEqual(companyName, actualResult);
        }

        [Then(@"type company equal type company from request creating company")]
        public void ThenTypeCompanyEqualTypeCompanyFromRequestCreatingCompany()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string actualResult = json["company"]["type"].ToString();
            Assert.AreEqual("ООО", actualResult);
        }

        [Then(@"users company equal users company which were in the request creating company")]
        public void ThenUsersCompanyEqualUsersCompanyWhichWereInTheRequestCreatingCompany()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string actualResult1 = json["company"]["users"][0]?.ToString();
            string actualResult2 = json["company"]["users"][1]?.ToString();
            Assert.AreEqual("possuum@mail.ru", actualResult1);
            Assert.AreEqual("testmail1@mail.ru", actualResult2);
        }

        // create a new user

        [Given(@"New user's datas like name, email, tasks, companies, inn is ready")]
        public void GivenNewUserSDatasLikeNameEmailTasksCompaniesInnIsReady()
        {
            string time = DateTime.Now.ToString();
            string temp = time.Replace(":", ".").Replace(" ", "").Replace("/", "").Replace("P", "p").Replace("M", "m");
            userName = temp + "userName";
            email = temp + "@gmail.com";

            newUserData.Add("email", email);
            newUserData.Add("inn", 123456789012);
            newUserData.Add("companies", new List<int> { 1182, 1183 });
            newUserData.Add("name", userName);
            newUserData.Add("tasks", new List<int> {7, 8});
        }

        [When(@"send request with valid datas for succesfull creating of user")]
        public void WhenSendRequestWithValidDatasForSuccesfullCreatingOfUser()
        {
            RestRequest request = new RestRequest("/tasks/rest/createuser", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(newUserData);
            response = client.Execute(request);
        }

        [Then(@"user'(.*)'s name from request creating of user")]
        public void ThenUserSNameFromRequestCreatingOfUser(string p0)
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string actualResult = json["name"].ToString();
            Assert.AreEqual(userName, actualResult);
        }

        [Then(@"user'(.*)'s email from request creating of user")]
        public void ThenUserSEmailFromRequestCreatingOfUser(string p0)
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string actualResult = json["email"].ToString();
            Assert.AreEqual(email, actualResult.ToLower());
        }

        [Then(@"user'(.*)'s tasks from request creating of user")]
        public void ThenUserSTasksFromRequestCreatingOfUser(string p0)
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string actualResult2 = json["tasks"][0]["name"].ToString();
            string actualResult = json["tasks"][1]["name"].ToString();
            Assert.AreEqual("Погладить белье", actualResult);
            Assert.AreEqual("Погладить белье", actualResult2);
        }

        [Then(@"user'(.*)'s companies from request creating of user")]
        public void ThenUserSCompaniesFromRequestCreatingOfUser(string p0)
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string actualResult = json["companies"][1]["id"].ToString();
            string actualResult2 = json["companies"][0]["id"].ToString();
            Assert.AreEqual("1182", actualResult);
            Assert.AreEqual("1183", actualResult2);
        }

        [Then(@"user'(.*)'s inn from request creating of user")]
        public void ThenUserSInnFromRequestCreatingOfUser(string p0)
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(email, email);
        }

        // Create an user with email which is already exist

        [Given(@"Data with existing email and valid name, tasks, companies for creating is ready")]
        public void GivenDataWithExistingEmailAndValidNameTasksCompaniesForCreatingIsReady()
        {
            email = "1020202010.55.29pm@gmail.com";
            string time = DateTime.Now.ToString();
            string temp = time.Replace(":", ".").Replace(" ", "").Replace("/", "").Replace("P", "p").Replace("M", "m");
            userName = temp + "userName";

            newUserData.Add("email", email);
            newUserData.Add("inn", 123456789012);
            newUserData.Add("companies", new List<int> { 1182, 1183 });
            newUserData.Add("name", userName);
            newUserData.Add("tasks", new List<int> { 7, 8 });
        }

        [When(@"send request with valid datas except email")]
        public void WhenSendRequestWithValidDatasExceptEmail()
        {
            RestRequest request = new RestRequest("/tasks/rest/createuser", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(newUserData);
            response = client.Execute(request);
        }

        [Then(@"server give answer to us with status response OK")]
        public void ThenServerGiveAnswerToUsWithStatusResponseOK()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [Then(@"server give answer type with value error")]
        public void ThenServerGiveAnswerTypeWithValueError()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("error", json["type"]?.ToString());
        }

        [Then(@"server give answer message like is email already exist")]
        public void ThenServerGiveAnswerMessageLikeIsEmailAlreadyExist()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("Пользователь с таким email уже существует ", json["message"]?.ToString());
        }

        //search count of peple which working in company

        [Given(@"data about company and users is ready for searching")]
        public void GivenDataAboutCompanyAndUsersIsReadyForSearching()
        {
            companyName = "EG_Larson Inc";
            userdata = new Dictionary<string, string>
            {
                {"query", companyName},
                {"fullSimilarity", "True" },
                {"partyType", "COMPANY" }
            };
        }

        [When(@"send request for searching users in my company")]
        public void WhenSendRequestForSearchingUsersInMyCompany()
        {
            RestRequest request = new RestRequest("/tasks/rest/magicsearch", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userdata);
            response = client.Execute(request);
        }

        [Then(@"the count of people which working in my company equal count working people in my company")]
        public void ThenTheCountOfPeopleWhichWorkingInMyCompanyEqualCountWorkingPeopleInMyCompany()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("11", json["foundCount"]?.ToString());
        }

        //Add avatar for user

        [When(@"I select an avatar for my account")]
        public void WhenISelectAnAvatarForMyAccount()
        {
            string userEmail = "werswerswerswers@gmail.com";
            RestRequest request = new RestRequest("/tasks/rest/addavatar/?email=" + userEmail, Method.POST);
            request.AddFile("avatar", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Src\taxatop.jpg"));
            response = client.Execute(request);
        }

        [Then(@"Server responce status OK")]
        public void ThenServerResponceStatusOK()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("ok", json["status"].ToString());
        }

        //Delete an avatar

        [When(@"I send request for delete an avatar from user account")]
        public void WhenISendRequestForDeleteAnAvatarFromUserAccount()
        {
            string email = "werswerswerswers@gmail.com";
            RestRequest request = new RestRequest("/tasks/rest/deleteavatar?email=" + email, Method.POST);
            response = client.Execute(request);
        }

        [Then(@"Server response status OK")]
        public void ThenServerResponseStatusOK()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("ok", json["status"].ToString());
        }
    }
}
