@api @auth @smoke
Feature: Authentication

@api @auth @smoke
Scenario: Customer can login with valid credentials
  Given the Fake Store API is available
  When I login with valid credentials
  Then the login response should be returned successfully
  And the login response should contain an auth token
  
@api @auth @negative
Scenario: Customer cannot login with invalid credentials
	Given the Fake Store API is available
	When I login with invalid credentials
	Then the login request should be rejected