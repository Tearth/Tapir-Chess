helm delete ingress-nginx -n ingress-nginx
helm delete postgresql -n postgresql
helm delete mongodb-games -n mongodb
helm delete mongodb-news -n mongodb
helm delete mongodb-players -n mongodb
helm delete rabbitmq -n rabbitmq

kubectl delete namespace ingress-nginx
kubectl delete namespace postgresql
kubectl delete namespace mongodb
kubectl delete namespace rabbitmq
kubectl delete namespace services

kubectl delete pv --all
kubectl delete pvc --all