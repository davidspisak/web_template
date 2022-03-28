DROP User If Exists templateuser;
CREATE User templateuser With Password 'templatepwd' NoCreateDB; 

GRANT USAGE ON SCHEMA aud To templateuser;
GRANT select, insert, update, delete On All Tables In Schema aud To templateuser;
GRANT usage On All Sequences In Schema aud To templateuser;