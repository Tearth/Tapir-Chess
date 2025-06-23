helm repo update

kubectl apply -f ./base/configs/
kubectl apply -f ./base/secrets/

helm upgrade ingress-nginx ingress-nginx/ingress-nginx --version 4.12.3 --values ./base/values/nginx-values.yaml -n ingress-nginx
helm upgrade postgresql cnpg/cloudnative-pg --values ./base/values/postgresql-values.yaml -n postgresql
helm upgrade mongodb-games bitnami/mongodb --version 16.5.20 --values ./base/values/mongodb-games-values.yaml -n mongodb
helm upgrade mongodb-news bitnami/mongodb --version 16.5.20 --values ./base/values/mongodb-news-values.yaml -n mongodb
helm upgrade mongodb-players bitnami/mongodb --version 16.5.20 --values ./base/values/mongodb-players-values.yaml -n mongodb
helm upgrade rabbitmq bitnami/rabbitmq --version 16.0.6 --values ./base/values/rabbitmq-values.yaml -n rabbitmq

kubectl apply -f ./base/clusters/
kubectl apply -f ./base/ingresses/
kubectl apply -f ./base/deployments/