DECLARE @CurrentSession AS INT = 18

USE WreckFestTrackSelector
SELECT 
    T.id,
    T.map_name,
    T.thumbnail_location,
    T.map_variant,
    T.track_surface,
    T.track_length,
    T.server_name,
    CASE WHEN ST.Track_id IS NOT NULL THEN 1 ELSE 0 END AS is_selected
FROM Tracks AS T
LEFT JOIN SelectedTracks AS ST
    ON T.id = ST.Track_id
    AND ST.bantha_session = @CurrentSession;