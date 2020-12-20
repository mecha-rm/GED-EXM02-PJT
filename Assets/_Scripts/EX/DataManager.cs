using System.Runtime.InteropServices;
using UnityEngine;

// data record struct used for data manager
// this is the same as the one in the DLL, except 'size' is not included.
// this is because 'data' already tracks the size.
public struct DataRecord
{
    public byte[] data; // size is saved in 'data' 
}

// data manager class
public class DataManager : MonoBehaviour
{
    // if 'true', the data is saved upon the destruction of this manager.
    public bool saveDataOnDestroy = false;
    // if 'true', the data is destroyed when the game object is deleted.
    public bool deleteDataOnDestroy = true;

    // dll
    private const string DLL_NAME = "GED-EXM02-DLL";

    // adds a data record
    [DllImport(DLL_NAME)]
    private static extern void AddDataRecord(byte[] data, int size);

    // inserts the record
    [DllImport(DLL_NAME)]
    private static extern void InsertDataRecord(int index, byte[] data, int size);

    // removes data record (does not delete data)
    [DllImport(DLL_NAME)]
    private static extern void RemoveDataRecord(byte[] data, int size);

    // removes a data record via its index
    [DllImport(DLL_NAME)]
    private static extern void RemoveDataRecordByIndex(int index);

    // clears out all data records. This does not delete the pointer data.
    [DllImport(DLL_NAME)]
    private static extern void ClearAllDataRecords();

    // deletes data record and removes it from the list.
    [DllImport(DLL_NAME)]
    private static extern void DeleteDataRecord(byte[] data, int size);

    // deletes data record via its index
    [DllImport(DLL_NAME)]
    private static extern void DeleteDataRecordByIndex(int index);

    // deletes all data records
    [DllImport(DLL_NAME)]
    private static extern void DeleteAllDataRecords();

    // checks to see if a provided data record is in the manager
    // maybe make this const like the DataManager function this plugin function is calling?
    [DllImport(DLL_NAME)]
    private static extern int ContainsDataRecord(byte[] data, int size);

    // gets the data at the requested index.
    // maybe find a way to return an array instead of fill an array
    [DllImport(DLL_NAME)]
    private static extern void GetData(int index, byte[] arr, int size);

    // returns the size of the record data
    // returns -1 if invalid index.
    [DllImport(DLL_NAME)]
    private static extern int GetDataSize(int index);

    // edits a data record's data, replacing it with newData.
    [DllImport(DLL_NAME)]
    private static extern void EditData(int index, byte[] newData);

    // edits the data record's size
    [DllImport(DLL_NAME)]
    private static extern void EditDataSize(int index, int newSize);

    // edits a data record, providing it with new values. This does not delete the existing data from memory.
    [DllImport(DLL_NAME)]
    private static extern void EditDataRecord(int index, byte[] newData, int newSize);

    // gets the amount of data records
    [DllImport(DLL_NAME)]
    private static extern int GetDataRecordCount();

    // checks to see if there are any data records
    [DllImport(DLL_NAME)]
    private static extern int HasDataRecords();

    // gets the file tied to this record system.
    [DllImport(DLL_NAME, EntryPoint = "GetFile")]
    private static extern System.IntPtr GetFile();

    // sets the file for this file system
    [DllImport(DLL_NAME)]
    private static extern void SetFile([MarshalAs(UnmanagedType.LPStr)] string file);

    // returns (1) if the set file is accessible, (0) if the set file is not accessible.
    [DllImport(DLL_NAME)]
    private static extern int FileAccessible();

    // imports the records from the provided file
    [DllImport(DLL_NAME)]
    private static extern int ImportDataRecords();

    // exports records to saved file.
    [DllImport(DLL_NAME)]
    private static extern int ExportDataRecords();

    // PUBLIC FUNCTIONS //
    // adds a data record
    public void AddDataRecordToManager(byte[] data, int size)
    {
        AddDataRecord(data, size);
    }

    // inserts the record
    public void InsertDataRecordIntoManager(int index, byte[] data, int size)
    {
        InsertDataRecord(index, data, size);
    }

    // removes data record (does not delete data)
    public void RemoveDataRecordFromManager(byte[] data, int size)
    {
        RemoveDataRecord(data, size);
    }

    // removes a data record via its index
    public void RemoveDataRecordFromManager(int index)
    {
        RemoveDataRecordByIndex(index);
    }

    // clears out all data records. This does not delete the pointer data.
    public void ClearAllDataRecordsFromManager()
    {
        ClearAllDataRecords();
    }

    // deletes data record and removes it from the list.
    public void DeleteDataRecordFromManager(byte[] data, int size)
    {
        DeleteDataRecord(data, size);
    }

    // deletes data record via its index
    public void DeleteDataRecordFromManager(int index)
    {
        DeleteDataRecordByIndex(index);
    }

    // deletes all data records
    public void DeleteAllDataRecordsFromManager()
    {
        DeleteAllDataRecords();
    }

    // checks if the data manager contains a record
    public bool ManagerContainsDataRecord(byte[] data, int size)
    {
        int res = ContainsDataRecord(data, size);
        return (res == 0) ? false : true;
    }

    // gets the data at the requested index.
    // maybe find a way to return an array instead of fill an array
    public byte[] GetDataFromManager(int index)
    {
        byte[] data = null;
        int size = GetDataSize(index);

        if (size > 0) // if there's data.
        {
            data = new byte[size];
            GetData(index, data, data.Length);
            return data;
        }
        else
        {
            return null;
        }

    }

    // returns the size of the record data
    // returns -1 if invalid index.
    public int GetDataSizeFromManager(int index)
    {
        return GetDataSize(index);
    }

    // edits a data record's data, replacing it with newData.
    public void EditDataInManager(int index, byte[] newData)
    {
        EditData(index, newData);
    }

    // edits the data record's size
    public void EditDataSizeInManager(int index, int newSize)
    {
        EditDataSize(index, newSize);
    }

    // edits a data record, providing it with new values.
    // This does not delete the existing data from memory.
    public void EditDataRecordInManager(int index, byte[] newData, int newSize)
    {
        EditDataRecord(index, newData, newSize);
    }

    // gets the amount of data records
    public int GetDataRecordAmount()
    {
        return GetDataRecordCount();
    }

    // checks to see if there are any data records
    public bool ManagerHasDataRecords()
    {
        int res = HasDataRecords();
        return (res == 0) ? false : true;
    }

    // gets the record file
    public string GetManagerFile()
    {
        return Marshal.PtrToStringAnsi(GetFile());
    }

    // sets the file for the data manager
    public void SetManagerFile(string file)
    {
        SetFile(file);
    }

    // checks to see if the set file is available for reading and writing.
    public bool FileAvailable()
    {
        int res = FileAccessible();
        return (res == 0) ? false : true;
    }

    // imports records from the set data file.
    public bool LoadDataRecords()
    {
        int res = ImportDataRecords();
        return (res == 0) ? false : true;
    }

    // exports records to saved file.
    public bool SaveDataRecords()
    {
        int res = ExportDataRecords();
        return (res == 0) ? false : true;
    }

    // CLASS START //

    // Start is called before the first frame update
    void Start()
    {

    }

    // adds a data record
    public void AddDataRecordToManager(DataRecord dr)
    {
        AddDataRecord(dr.data, dr.data.Length);
    }

    // inserts the record
    public void InsertDataRecordIntoManager(int index, DataRecord dr)
    {
        InsertDataRecord(index, dr.data, dr.data.Length);
    }

    // removes data record (does not delete data)
    public void RemoveDataRecordFromManager(DataRecord dr)
    {
        RemoveDataRecord(dr.data, dr.data.Length);
    }

    // deletes data record and removes it from the list.
    public void DeleteDataRecordFromManager(DataRecord dr)
    {
        DeleteDataRecord(dr.data, dr.data.Length);
    }

    // checks if the data manager contains a record
    public bool ManagerContainsDataRecord(DataRecord dr)
    {
        return ManagerContainsDataRecord(dr.data, dr.data.Length);
    }

    // gets the data at the requested index.
    // maybe find a way to return an array instead of fill an array
    public DataRecord GetDataRecordFromManager(int index)
    {
        byte[] data = GetDataFromManager(index);
        DataRecord dr;

        dr.data = data;
        return dr;
    }

    // edits a data record, providing it with new values.
    // This does not delete the existing data from memory.
    public void EditDataRecordInManager(int index, DataRecord dr)
    {
        EditDataRecord(index, dr.data, dr.data.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnDestroy is called when the object is being destroyed.
    private void OnDestroy()
    {
        // if data should be saved.
        if (saveDataOnDestroy)
            ExportDataRecords();

        // if data should be deleted when this object is destroyed.
        // calling DeleteAllDataRecords() crashes the project.
        // so clear is called instead, though it may be leaving data unaccounted for.
        if (deleteDataOnDestroy)
            ClearAllDataRecords(); // delete all records// DeleteAllDataRecords(); // delete all records
    }
}
