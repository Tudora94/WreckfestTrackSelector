USE WreckFestTrackSelector
CREATE TABLE SelectedTracks (
Track_id UNIQUEIDENTIFIER,
selected_by_ip VARCHAR(50) NULL,
selected_by_browser_id VARCHAR(100) NULL,
selected_at DATETIME NULL,
bantha_session INT
)