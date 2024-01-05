#!/usr/bin/env bash
# exit when any command fails
set -e

TENANT="c2170b64-c4b3-4d4d-996f-1d1565557252"
SUBSCRIPTION="72577fc5-ca6e-4c91-a1da-6e3d92d63524"
LOCATION="brazilsouth"

RESOURCEGROUP="ecommerce"
STORAGEACCOUNTNAME="ecommercestorageaccount"
APPNAME="ecommercepostechapp"

do_login()
{
    echo "Start Login"
    subscriptionId="$(az account list --query "[?isDefault].id" -o tsv)"
    if [ $subscriptionId != $SUBSCRIPTION ]; then
        az login --tenant $TENANT
        az account set --subscription $SUBSCRIPTION
    fi
    echo "End Login"
}

do_resource_group()
{
    echo "Checking Resource Groups"
    if [ $(az group exists --name $RESOURCEGROUP) = false ]; then
        az group create --name $RESOURCEGROUP --location $LOCATION
    fi
    echo "End Resource Group"
}

do_function_app()
{
    echo "Start Function App"
    az deployment group create \
    --name CheckoutFunctionApp \
    --resource-group $RESOURCEGROUP \
    --template-file azuredeploy.json \
    --parameters azuredeploy.parameters.json \
    --verbose
    echo "End function App"
}

main()
{
    do_login
    do_resource_group
    do_function_app
}

# Start
main
# End