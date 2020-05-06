Feature: List all Users

@mytag
Scenario: List all Users
	Given I have URI,URL and Content Type
	When I perform GET method
	Then I should get OK response with all the user listed