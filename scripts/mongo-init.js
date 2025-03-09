db = db.getSiblingDB('news');
db.createUser({
    user: "tapir",
    pwd: "JnhILirAtXrwILs",
    roles: [{
        role: "readWrite",
        db: "news"
    }]
});