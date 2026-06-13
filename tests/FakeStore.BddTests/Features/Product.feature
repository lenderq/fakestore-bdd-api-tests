@api @products @smoke
Feature: Product catalog

@api @products @smoke
Scenario: Customer can receive the product catalog
    Given the Fake Store API is available
    When I request the product catalog
    Then the product catalog should be returned successfully
	And the product catalog should contain valid product information
	
@api @products @smoke
Scenario: Customer can receive a single product by id
	Given the Fake Store API is available
	When I request product with id 1
	Then the product should be returned successfully
	And the product id should be 1
	And the product should contain valid product information
	
@api @products @smoke
Scenario: Customer can limit the product catalog result
	Given the Fake Store API is available
	When I request the product catalog with limit 5
	Then the product count should be 5
	And the product catalog should contain valid product information
	
@api @products @smoke
Scenario: Customer can receive product categories
	Given the Fake Store API is available
	When I request product categories
	Then the product categories should be returned successfully
	And the product categories should contain valid information
	
@api @products @smoke
Scenario: Customer can create a product
	Given the Fake Store API is available
	When I create a product
	Then the product should be created successfully
	And the product should contain reliable information sent about the product
	
@api @products @regression
Scenario: Customer can update a product
	Given the Fake Store API is available
	When I update a product with id 1
	Then the product should be returned successfully
	And the product id should be 1
	And the product should contain reliable information sent about the product
	
@api @products @regression
Scenario: Customer can delete a product
	Given the Fake Store API is available
	When I delete a product with id 1
	Then the product should be returned successfully
	And the product id should be 1