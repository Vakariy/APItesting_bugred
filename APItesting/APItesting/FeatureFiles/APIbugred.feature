Feature: APIbugredSite
	In order to have my account with personal information
	As a user
	I want to have opportunity register in the web-site

	Background:  
	Given create of a new client 

@regist
Scenario: registration 
	Given dates like e-mail of user, name of user and password of user is ready for create a new account
	When send request with valid datas for succesfull registration
	Then account has been created
	Then user name which we get from request equal user name which we sended by request registrarion
	Then e-mail of user which we get from request equal e-mail of user which we sended by request registrarion


@negativeRegistration
Scenario: Register an account with existing email
  Given Data with existing email for registretion is ready
  When I send POST request with prepared data
  Then Server status response OK
  Then Server response type is error
  Then Server response message is email already exist
	

@createNewCompany
Scenario Outline: Create a new company 
	Given Data of company like name, type, users, email owner is ready 
	When send request with valid datas for succesfull create company
	Then company has been created	
	Then name company equal name company from request creating company
	Then type company equal type company from request creating company
	Then users company equal users company which were in the request creating company


@createNewUser
Scenario: Create a new user
	Given New user's datas like name, email, tasks, companies, inn is ready
	When send request with valid datas for succesfull creating of user
	Then user's name equal user's name from request creating of user
	Then user's email equal user's email from request creating of user
	Then user's tasks equal user's tasks from request creating of user
	Then user's companies equal user's companies from request creating of user
	Then user's inn equal user's inn from request creating of user

@createUserExistEmail
Scenario: create a user with exist email
	Given Data with existing email and valid name, tasks, companies for creating is ready
	When send request with valid datas except email
	Then server give answer to us with status response OK
	Then server give answer type with value error
	Then server give answer message like is email already exist

@searchUserInCompany
Scenario: Search all users whick work in my company
	Given data about company and users is ready for searching
	When send request for searching users in my company
	Then the count of people which working in my company equal count working people in my company
	
@add_avatar
Scenario: Add an avatar for a user
	When I select an avatar for my account
	Then Server responce status OK

@delete_avatar
Scenario: Delete an avatar from user account
	When I send request for delete an avatar from user account
	Then Server response status OK 
