using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Binary formatter with attached serialization surrogates
/// for Unity based data serialization.
/// </summary>
public class UnityBinaryFormatter
{
    private BinaryFormatter Formatter;

    /// <summary>
    /// Initializes an instance of <see cref="UnityBinaryFormatter"/>
    /// </summary>
    public UnityBinaryFormatter()
    {
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        var vector3SS = new Vector3SerializationSurrogate();
        var quaternionSS = new QuaternionSerializationSurrogate();
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
        surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSS);

        Formatter = new BinaryFormatter()
        {
            SurrogateSelector = surrogateSelector
        };
    }

    /// <summary>
    /// Serializes the passed data into the filestream.
    /// </summary>
    /// <param name="stream">The file stream to write to</param>
    /// <param name="obj">The data to serialize</param>
    public void Serialize(FileStream stream, object obj) => Formatter.Serialize(stream, obj);

    /// <summary>
    /// Deserializes the data from the passed filestream.
    /// </summary>
    /// <param name="stream">The file stream to read from</param>
    /// <returns>The deserialized data</returns>
    public object Deserialize(FileStream stream) => Formatter.Deserialize(stream);
}

/// <summary>
/// Unity Vector3 serialization surrogate
/// </summary>
public class Vector3SerializationSurrogate : ISerializationSurrogate
{
    /// <inheritdoc/>
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var vector3 = (Vector3)obj;
        info.AddValue("x", vector3.x);
        info.AddValue("y", vector3.y);
        info.AddValue("z", vector3.z);
    }

    /// <inheritdoc/>
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        var vector3 = (Vector3)obj;
        vector3.x = (float)info.GetValue("x", typeof(float));
        vector3.y = (float)info.GetValue("y", typeof(float));
        vector3.z = (float)info.GetValue("z", typeof(float));
        obj = vector3;
        return obj;
    }
}

public class QuaternionSerializationSurrogate : ISerializationSurrogate
{
    /// <inheritdoc/>
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {

        Quaternion quat = (Quaternion)obj;
        info.AddValue("x", quat.x);
        info.AddValue("y", quat.y);
        info.AddValue("z", quat.z);
        info.AddValue("w", quat.w);
    }

    /// <inheritdoc/>
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Quaternion quat = (Quaternion)obj;
        quat.x = (float)info.GetValue("x", typeof(float));
        quat.y = (float)info.GetValue("y", typeof(float));
        quat.z = (float)info.GetValue("z", typeof(float));
        quat.w = (float)info.GetValue("w", typeof(float));
        obj = quat;
        return obj;
    }
}