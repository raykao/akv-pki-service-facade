# Example App

The following environment variables are required to access AKV

```bash
AZURE_CLIENT_ID=""
AZURE_CLIENT_SECRET=""
AZURE_TENANT_ID=""
```

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