let databases = ['news', 'players', 'games'];

databases.forEach((name) => {
    db = db.getSiblingDB(name);
    db.createUser({
        user: "tapir",
        pwd: "JnhILirAtXrwILs",
        roles: [{
            role: "readWrite",
            db: name
        }]
    });
    db.events.createIndex({ "AggregateId": 1 });
    db.events.createIndex({ "Timestamp": 1 });
});