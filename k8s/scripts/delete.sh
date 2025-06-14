helm delete ingress-nginx -n ingress-nginx
helm delete mongodb -n infrastructure
helm delete postgresql -n infrastructure
helm delete rabbitmq -n infrastructure

kubectl delete namespace ingress-nginx
kubectl delete namespace services
kubectl delete namespace infrastructure

kubectl delete ingressclass --all