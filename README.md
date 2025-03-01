# eCommerce

## Overview

This project is a microservices-based eCommerce platform built using C# and Docker. It leverages Kubernetes for orchestration and RabbitMQ for messaging, ensuring a scalable and efficient system.

## Microservices Architecture

The solution comprises the following microservices:

- **Cart API (`eCommerce.Cart.API`)**: Manages user shopping carts.
- **Inventory API (`eCommerce.Inventory.API`)**: Handles product inventory and stock levels.
- **Notification API (`eCommerce.Notification.API`)**: Sends notifications to users about order statuses and promotions.
- **Product API (`eCommerce.Product.API`)**: Provides product information and listings.
- **Product Detail API (`eCommerce.ProductDetail.API`)**: Offers detailed information on individual products.
- **Ocelot API Gateway (`eCommerce.OcelotApiGateway`)**: Acts as a unified entry point to the microservices, managing request routing.

## Infrastructure Components

- **RabbitMQ (`eCommerce.RabbitMq`)**: Facilitates asynchronous communication between microservices.
- **Kubernetes Scripts (`K8sScripts`)**: Contains Kubernetes deployment and service configuration files for orchestrating the microservices.

## Prerequisites

Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Kubernetes](https://kubernetes.io/docs/setup/) (or [Minikube](https://minikube.sigs.k8s.io/docs/start/) for local development)
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [Helm](https://helm.sh/docs/intro/install/)

## Getting Started

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/aklazy17/eCommerce.git
   cd eCommerce

2. **Build and Push Docker Images**:
   For each microservice, navigate to its directory and build the Docker image:

   ```bash
   docker build -t your-dockerhub-username/microservice-name .
   ```
   Push the image to Docker Hub:
   
   ```bash
   docker push your-dockerhub-username/microservice-name
   ```
   Repeat these steps for all microservices.

3. Deploy to Kubernetes:
   
   Navigate to the K8sScripts directory:
   
   ```bash
   cd K8sScripts
   ```

   Apply the Kubernetes configurations:

   ```bash
   kubectl apply -f .

4. **Access the Application**:
   Use `kubectl get services` to retrieve the external IP addresses and ports assigned to your services. Access the application through the API Gateway's external IP.

## Configuration

- **Environment Variables**: Each microservice may require specific environment variables (e.g., database connection strings, RabbitMQ settings). Configure these in the Kubernetes deployment files or use Kubernetes Secrets and ConfigMaps.

- **RabbitMQ**: Ensure RabbitMQ is deployed and accessible to the microservices. You can deploy RabbitMQ using Helm:

```bash
helm repo add bitnami https://charts.bitnami.com/bitnami
helm install rabbitmq bitnami/rabbitmq
```

## Acknowledgements
- [**Ocelot**](https://ocelot.readthedocs.io/en/latest/) for the API Gateway implementation.
- [**RabbitMQ**](https://www.rabbitmq.com/) for message brokering.
- [**Kubernetes**](https://kubernetes.io/) for orchestration.
