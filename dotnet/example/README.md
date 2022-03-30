# Example App


## Environment Variables
The following environment variables are required to access AKV is using a Service Principal.

```bash
AZURE_CLIENT_ID=""
AZURE_CLIENT_SECRET=""
AZURE_TENANT_ID=""
```

You may also provide a client certificate via the ```AZURE_CLIENT_CERTIFICATE_PATH``` environment variable instead of ```AZURE_CLIENT_SECRET```.

## Azure Key Vault Policy
*Note*: the Service Principal (client_id) being used must also have the proper Key Vault Policies:
- Certificate:
	- Get
	- Create
	- Update
- Keys:
	- Get
	- Create
	- Update
	- Cryptographic Operations
		- Sign

Since we are using the Default Identity (Azure.Core) SDK you could also use store User Credentials or an Managed Service Identity (MSI).  The same rules apply regarding Key Vault Policies, and the Identity Provider will attempt to access the the KV using the default order (User, SP, MSI). 

## Using app
export the above Environment Variables do the usual dotnet song/dance ```dotnet run```.

Once complete the terminal output will give an example to use ```openssl``` to verify the RootCA certificate (```rootCa.pem```) and the example client cert (```clientCert.pem```);

```bash
openssl verify -verbose -CAfile rootCa.pem clientCert.pem
```