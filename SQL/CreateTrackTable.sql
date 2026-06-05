USE WreckFestTrackSelector
CREATE TABLE Tracks (
id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
map_name VARCHAR(255) NOT NULL,
thumbnail_location NVARCHAR(600) NOT NULL,
map_variant VARCHAR(255) NOT NULL,
track_surface VARCHAR(255) NOT NULL,
track_length INT NOT NULL,
server_name VARCHAR(255) NOT NULL
)
GO