param name string
param location string = resourceGroup().location
param containerAppEnvironmentId string
param repositoryImage string
param envVars array = []
param allowExternalIngress bool = false
param allowInternalIngress bool = true
param targetIngressPort int = 80
param registry string
param registryUsername string
param daprComponents array = []
@secure()
param registryPassword string

resource containerApp 'Microsoft.Web/containerApps@2021-03-01' = {
  name: name
  kind: 'containerapp'
  location: location
  properties: {
    kubeEnvironmentId: containerAppEnvironmentId
    configuration: {
      secrets: [
        {
          name: 'container-registry-password'
          value: registryPassword
        }
      ]      
      registries: [
        {
          server: registry
          username: registryUsername
          passwordSecretRef: 'container-registry-password'
        }
      ]
      ingress: {
        internal: allowInternalIngress
        external: allowExternalIngress
        targetPort: targetIngressPort
      }
    }
    template: {
      containers: [
        {
          image: repositoryImage
          name: name
          env: envVars
        }
      ]
      scale: {
        minReplicas: 0
      }
      dapr: {
        enabled: true
        appId: name
        appPort: targetIngressPort
        components: daprComponents
      }
    }
  }
}

output fqdn string = containerApp.properties.configuration.ingress.fqdn
