CREATE TABLE Players (
  Id UUID PRIMARY KEY NOT NULL,
  CreatedAt TIMESTAMP NOT NULL,
  UpdatedAt TIMESTAMP NULL,
  Username TEXT NULL,
  Email TEXT NULL,
  Country TEXT NULL,
  AboutMe TEXT NULL
);