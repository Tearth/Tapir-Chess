CREATE TABLE News (
  Id SERIAL PRIMARY KEY,
  AggregateId UUID NOT NULL,
  CreatedAt TIMESTAMP NOT NULL,
  UpdatedAt TIMESTAMP NULL,
  DeletedAt TIMESTAMP NULL,
  Title TEXT NULL,
  Alias TEXT NULL,
  Content TEXT NULL,
  Deleted BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE INDEX news_aggregateid_idx ON News (AggregateId);