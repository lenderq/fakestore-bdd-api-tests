@api @users @smoke
Feature: Users

@api @users @smoke
Scenario: Customer can receive the user list
  Given the Fake Store API is available
  When I request the user list
  Then the user list should be returned successfully
  And the user list should contain valid information 

@api @users @smoke
Scenario: Customer can receive a single user by id
	Given the Fake Store API is available
	When I request the user with id 1
	Then the user should be returned successfully
	And the user id should be 1
	And the user should contain valid information