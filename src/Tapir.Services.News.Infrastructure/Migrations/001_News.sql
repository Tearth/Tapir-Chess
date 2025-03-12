CREATE TABLE News (
  Id SERIAL PRIMARY KEY,
  AggregateId UUID NOT NULL,
  CreatedAt TIMESTAMP NOT NULL,
  UpdatedAt TIMESTAMP NULL,
  Title TEXT NULL,
  Alias TEXT NULL,
  Content TEXT NULL
);

CREATE INDEX news_aggregateid_idx ON News (AggregateId);