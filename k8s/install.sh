helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

kubectl create namespace ingress-nginx
kubectl create namespace postgresql
kubectl create namespace mongodb
kubectl create namespace rabbitmq
kubectl create namespace services

kubectl apply -f ./configs/
kubectl apply -f ./secrets/

helm install ingress-nginx ingress-nginx/ingress-nginx --version 4.12.3 --values ./values/nginx-values.yaml -n ingress-nginx
helm install postgresql cnpg/cloudnative-pg --values ./values/postgresql-values.yaml -n postgresql
helm install mongodb-games bitnami/mongodb --version 16.5.20 --values ./values/mongodb-games-values.yaml -n mongodb
helm install mongodb-news bitnami/mongodb --version 16.5.20 --values ./values/mongodb-news-values.yaml -n mongodb
helm install mongodb-players bitnami/mongodb --version 16.5.20 --values ./values/mongodb-players-values.yaml -n mongodb
helm install rabbitmq bitnami/rabbitmq --version 16.0.6 --values ./values/rabbitmq-values.yaml -n rabbitmq

sleep 60

kubectl apply -f ./clusters/
kubectl apply -f ./ingress/
kubectl apply -f ./deployments/