#!/bin/bash
resource_group=DockerDemoRG1
vm_name=DockerDemoVM
vm_port=8080

az group create --location northeurope --name $resource_group

az vm create --name $vm_name --resource-group $resource_group \
			--image Ubuntu2204 --size Standard_B1s \
			--generate-ssh-keys \
			--custom-data @cloud-init_docker.yaml \
			--admin-username azureuser

# Open port 8080 for web traffic (if needed)
az vm open-port --port $vm_port --resource-group $resource_group --name $vm_name