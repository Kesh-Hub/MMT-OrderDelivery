# MMT-OrderDelivery

## Improvements
If there was more time to be spent on this API, I would consider doing the following changes:
- Versioning the API
  - This would allow changes to be made to the API without breaking backwards compatibility. The preferred approach would be using Accept Headers where the client will request which version they want to use.
- Handle orders placed on the same day
  - If there are multiple orders placed on the same day by a customer, the API would return the first one it finds in the Database. This does not mean it is the latest order, it could be the first order placed on that day.
  - One easy way to fix this would be to save the timestamp alongside the date in the OrderDate database field. The API will then automatically find the latest order based on this field.
- Improve Unit Tests to use Builder patterns
  - As the API grows, there might be code duplication when creating Mock Models for the Unit Tests. This can be avoided by using Builders and Fixtures in the Unit Test project.

## Changes required before running on Production environment
- The Web.config contains the key and connection strings which enable the API to work. However it is not ideal to leave these configurations in that file. These should be removed and set up on Azure Configuration Manager or Azure Key Vault. The architecture to check the AppConfigs from Azure has already been set up in [ConfigurationManager.cs](https://github.com/Kesh-Hub/MMT-OrderDelivery/blob/master/OrderDelivery/Utils/ConfigurationManager.cs)
- The Trace logs need to be redirected to Application Insights. This can be done by using an override method to the existing Trace Method, thus avoiding changing the logging in all files.
- Create ARM template to facilitate deployment of API.

## Getting Started
- Open this Git project using Visual Studio
- Refresh Nuget packages to ensure everything is installed correctly. Required packages are EntityFramework6 and AutoFac.
- Set OrderDelivery as the Startup project and run locally.
- Using Postman (or alternative) send HTTP POST request to https://localhost:44386/api/OrderDetails.
  - Body should contain JSON in the following format:
  ```json
    { 
	    "user": "cat.owner@mmtdigital.co.uk",
	    "customerId": "C34454"
    }
    ```
- API should return the expected results. 
Note that on the first local run, the API can take up to 1 min to generate results. This is due to an EF cold-start. Future API calls should take around 150ms to execute.


