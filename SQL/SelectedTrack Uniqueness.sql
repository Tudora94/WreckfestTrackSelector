ALTER TABLE SelectedTracks
ADD CONSTRAINT UQ_SelectedTracks_Browser_Session
UNIQUE (selected_by_browser_id, bantha_session);

ALTER TABLE SelectedTracks
ADD CONSTRAINT UQ_SelectedTracks_IP_Session
UNIQUE (selected_by_ip, bantha_session);