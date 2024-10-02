/*CREATE TABLE Songs (
    id INTEGER PRIMARY KEY AUTOINCREMENT,  -- Uso de AUTOINCREMENT en SQLite
    title VARCHAR(255) NOT NULL
);

CREATE TABLE Images (
    id_internal INTEGER,  -- No se puede usar AUTOINCREMENT en una clave primaria compuesta
    id_song INTEGER,
    url VARCHAR(255) NOT NULL,
    FOREIGN KEY (id_song) REFERENCES Song(id),
    PRIMARY KEY (id_internal, id_song)  -- Definir la clave primaria compuesta
);*/




SELECT * FROM Songs;
SELECT * FROM Images;

