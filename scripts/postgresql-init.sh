#!/bin/bash

set -e
set -u

function create_user_and_database() {
	local database=$1
	echo "  Creating user and database '$database'"
	psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
	    CREATE DATABASE $database;
	    CREATE USER $database WITH PASSWORD 'Test123!';
		ALTER DATABASE $database OWNER TO $database;
EOSQL
}

if [ -n "$POSTGRES_MULTIPLE_DB" ]; then
	echo "Multiple database creation requested: $POSTGRES_MULTIPLE_DB"
	for db in $(echo $POSTGRES_MULTIPLE_DB | tr ',' ' '); do
		create_user_and_database $db
	done
	echo "Multiple databases created"
fi
