helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

kubectl create namespace ingress-nginx
kubectl create namespace services
kubectl create namespace infrastructure

kubectl apply -f ../secrets/mongodb-secret.yaml -n infrastructure
kubectl apply -f ../secrets/postgresql-init.yaml -n infrastructure
kubectl apply -f ../secrets/postgresql-secret.yaml -n infrastructure
kubectl apply -f ../secrets/rabbitmq-init.yaml -n infrastructure
kubectl apply -f ../secrets/rabbitmq-secret.yaml -n infrastructure

helm install ingress-nginx ingress-nginx/ingress-nginx --version 4.12.3 --values ../nginx-values.yaml -n ingress-nginx
helm install mongodb bitnami/mongodb --version 16.5.20 --values ../mongodb-values.yaml -n infrastructure
helm install postgresql bitnami/postgresql --version 16.7.10 --values ../postgresql-values.yaml -n infrastructure
helm install rabbitmq bitnami/rabbitmq --version 16.0.6 --values ../rabbitmq-values.yaml -n infrastructure

kubectl apply -f ../configs/games-config.yaml -n services
kubectl apply -f ../configs/gateway-config.yaml -n services
kubectl apply -f ../configs/identity-config.yaml -n services
kubectl apply -f ../configs/news-config.yaml -n services
kubectl apply -f ../configs/players-config.yaml -n services

kubectl apply -f ../secrets/games-secret.yaml -n services
kubectl apply -f ../secrets/gateway-secret.yaml -n services
kubectl apply -f ../secrets/identity-secret.yaml -n services
kubectl apply -f ../secrets/news-secret.yaml -n services
kubectl apply -f ../secrets/players-secret.yaml -n services

kubectl apply -f ../nginx-ingress.yaml -n services
kubectl apply -f ../gateway-deployment.yaml -n services
kubectl apply -f ../games-deployment.yaml -n services
kubectl apply -f ../identity-deployment.yaml -n services
kubectl apply -f ../news-deployment.yaml -n services
kubectl apply -f ../players-deployment.yaml -n services