helm repo update

helm upgrade ingress-nginx ingress-nginx/ingress-nginx --version 4.12.3 -n ingress-nginx
helm upgrade kube-prometheus prometheus-community/kube-prometheus-stack --version 75.6.0 -n monitoring
helm upgrade loki grafana/loki --version 6.30.1 --values ./base/values/loki-values.yaml -n loki
helm upgrade postgresql cnpg/cloudnative-pg --version 0.24.0 --values ./base/values/postgresql-values.yaml -n postgresql
helm upgrade mongodb-games bitnami/mongodb --version 16.5.20 --values ./base/values/mongodb-games-values.yaml -n mongodb
helm upgrade mongodb-news bitnami/mongodb --version 16.5.20 --values ./base/values/mongodb-news-values.yaml -n mongodb
helm upgrade mongodb-players bitnami/mongodb --version 16.5.20 --values ./base/values/mongodb-players-values.yaml -n mongodb
helm upgrade rabbitmq bitnami/rabbitmq --version 16.0.6 --values ./base/values/rabbitmq-values.yaml -n rabbitmq

kubectl apply -k ./base/