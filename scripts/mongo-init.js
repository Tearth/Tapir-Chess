print('===================================');
print('MongoDB initialization script START');
print('===================================');

db = db.getSiblingDB('news');
db.createUser({
    user: "tapir",
    pwd: "JnhILirAtXrwILs",
    roles: [{
        role: "readWrite",
        db: "news"
    }]
});

print('=================================');
print('MongoDB initialization script END');
print('=================================');