db = db.getSiblingDB('news');
db.createUser({
    user: "tapir",
    pwd: "JnhILirAtXrwILs",
    roles: [{
        role: "readWrite",
        db: "news"
    }]
});
db.events.createIndex({ "AggregateId": 1 });
db.events.createIndex({ "Timestamp": 1 });