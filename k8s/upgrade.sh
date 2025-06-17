helm repo update

kubectl apply -f ./configs/
kubectl apply -f ./secrets/

helm upgrade ingress-nginx ingress-nginx/ingress-nginx --version 4.12.3 --values ./values/nginx-values.yaml -n ingress-nginx
helm upgrade postgresql cnpg/cloudnative-pg --values ./values/postgresql-values.yaml -n postgresql
helm upgrade mongodb-games bitnami/mongodb --version 16.5.20 --values ./values/mongodb-games-values.yaml -n mongodb
helm upgrade mongodb-news bitnami/mongodb --version 16.5.20 --values ./values/mongodb-news-values.yaml -n mongodb
helm upgrade mongodb-players bitnami/mongodb --version 16.5.20 --values ./values/mongodb-players-values.yaml -n mongodb
helm upgrade rabbitmq bitnami/rabbitmq --version 16.0.6 --values ./values/rabbitmq-values.yaml -n rabbitmq

kubectl apply -f ./clusters/
kubectl apply -f ./ingresses/
kubectl apply -f ./deployments/