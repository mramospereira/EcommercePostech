#!/usr/bin/env bash
# exit when any command fails
set -e

APPNAME="ecommercepostechapp"

cd ../
func azure functionapp publish $APPNAME
