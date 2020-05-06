Feature: User Registration
	
@mytag
Scenario Outline: Register User
	Given I have URI, URL , ContentType and '<Payload>'
	When I invoke POST method
	Then I should get <ResponseCode> with response
	Examples:
| Payload                    | ResponseCode |
| Register_User              | OK           |
| Register_User_Unsuccessful | BadRequest   |