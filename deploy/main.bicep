param location string = resourceGroup().location

param catalog_api_image string
param orders_api_image string
param ui_image string
param registry string
param registryUsername string

@secure()
param registryPassword string

module env 'environment.bicep' = {
  name: 'containerAppEnvironment'
  params: {
    location: location
  }
}

module catalog_api 'container-app.bicep' = {
  name: 'catalog'
  params: {
    name: 'catalog'
    containerAppEnvironmentId: env.outputs.id
    registry: registry
    registryPassword: registryPassword
    registryUsername: registryUsername
    repositoryImage: catalog_api_image
  }
}

module orders_api 'container-app.bicep' = {
  name: 'orders'
  params: {
    name: 'orders'
    containerAppEnvironmentId: env.outputs.id
    registry: registry
    registryPassword: registryPassword
    registryUsername: registryUsername
    repositoryImage: orders_api_image
  }
}

module ui 'container-app.bicep' = {
  name: 'frontend'
  params: {
    name: 'frontend'
    containerAppEnvironmentId: env.outputs.id
    registry: registry
    registryPassword: registryPassword
    registryUsername: registryUsername
    repositoryImage: ui_image
    envVars : [
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Development'
      }
    ]
  }
}
