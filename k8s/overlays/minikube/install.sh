helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo add cnpg https://cloudnative-pg.github.io/charts
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update

kubectl create namespace ingress-nginx
kubectl create namespace monitoring
kubectl create namespace loki
kubectl create namespace postgresql
kubectl create namespace mongodb
kubectl create namespace rabbitmq
kubectl create namespace services

helm install ingress-nginx ingress-nginx/ingress-nginx --version 4.12.3 --values ../../base/values/nginx-values.yaml -n ingress-nginx
helm install kube-prometheus prometheus-community/kube-prometheus-stack --version 75.6.0 --values ../../base/values/kube-prometheus-values.yaml -n monitoring
helm install loki grafana/loki --version 6.30.1 --values ../../base/values/loki-values.yaml --values ./values/loki-values.yaml -n loki
helm install postgresql cnpg/cloudnative-pg --version 0.24.0 --values ../../base/values/postgresql-values.yaml --values ./values/postgresql-values.yaml -n postgresql
helm install mongodb-games bitnami/mongodb --version 16.5.20 --values ../../base/values/mongodb-games-values.yaml --values ./values/mongodb-games-values.yaml -n mongodb
helm install mongodb-news bitnami/mongodb --version 16.5.20 --values ../../base/values/mongodb-news-values.yaml --values ./values/mongodb-news-values.yaml -n mongodb
helm install mongodb-players bitnami/mongodb --version 16.5.20 --values ../../base/values/mongodb-players-values.yaml --values ./values/mongodb-players-values.yaml -n mongodb
helm install rabbitmq bitnami/rabbitmq --version 16.0.6 --values ../../base/values/rabbitmq-values.yaml --values ./values/rabbitmq-values.yaml -n rabbitmq

kubectl create secret tls tls-secret --key ../../base/secrets/certificates/tapirchess.dev.key --cert ../../base/secrets/certificates/tapirchess.dev.crt -n services

sleep 60

kubectl apply -k .