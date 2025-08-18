// ...existing code...
if (mode == Mode.Record)
{
    string path = Path.Combine(Application.persistentDataPath, "input_record.json");
    // 덮어쓰기 모드로 파일 저장
    File.WriteAllText(path, JsonUtility.ToJson(recordedInputs));
}
// ...existing code...

