@api @carts @smoke
Feature: Shopping carts

@api @carts @smoke
Scenario: Customer can receive the cart list
	Given the Fake Store API is available
	When I request the cart list
	Then the cart list should be returned successfully
	And the cart list should contain valid information
	
@api @carts @smoke
Scenario: Customer can receive a single cart by id
	Given the Fake Store API is available
	When I request the single cart with id 1
	Then the cart should be returned successfully
	And the cart id should be 1
	And the cart should contain valid information
	
@api @carts @smoke
Scenario: Customer can receive carts by user id
	Given the Fake Store API is available
	When I request the cart list with user id 1
	Then the cart list should be returned successfully
	And all returned carts should belong to user 1
	And the cart list should contain valid information