using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi;
using System.Security.Cryptography;
using UnityEngine.Events;
using System.Linq;
public class IfPrivateKeyFoundNearUseItMono : MonoBehaviour
{
    public Eloi.AbstractMetaAbsolutePathFileMono m_nearFile;
    public UnityEvent<string> m_onPrivateKeyFound;
    public UnityEvent<string> m_onPublicKeyFound;
    public UnityEvent m_onKeyPairFoundAndLoaded;
    public UnityEvent m_onPrivateKeyNotFound;
    public string m_publicKey;
    public string m_debugFirstPartOfPrivateKey;

    public void Awake()
    {
        LoadPrivateKeyNearIfThere();
    }

    [ContextMenu("Load Private key if there")]
    public void LoadPrivateKeyNearIfThere()
    {
        m_publicKey = "";   
        m_debugFirstPartOfPrivateKey = "";
        string privateKey = "";
        m_nearFile.GetPath(out string path);
        bool exist = System.IO.File.Exists(path);
        if (!exist)
        {
            Eloi.E_FileAndFolderUtility.ExportByOverriding(m_nearFile, "");
            m_onPrivateKeyNotFound.Invoke();
            return;
        }

        if (System.IO.File.Exists(path))
        {
            string content = System.IO.File.ReadAllText(path);

            RSA rsa = RSA.Create(1024);


            rsa.FromXmlString(content);
            if (rsa is RSACryptoServiceProvider)
            {
                RSACryptoServiceProvider rsaCsp = (RSACryptoServiceProvider)rsa;
                privateKey = rsaCsp.ToXmlString(true);
                if (privateKey.Length > 50)
                    m_debugFirstPartOfPrivateKey = privateKey.Substring(0, 50);
                m_publicKey = rsaCsp.ToXmlString(false);
                m_onPublicKeyFound.Invoke(m_publicKey);
            }
        }
        if (privateKey.Length > 0)
            m_onPrivateKeyFound.Invoke(privateKey);
        else
            m_onPrivateKeyNotFound.Invoke();
        m_onKeyPairFoundAndLoaded.Invoke();
    }

    [ContextMenu("Generate Default RSA KEY")]
    public void GenerateDefaultRSA() { 
    
        RSA rsa = RSA.Create(1024);
        string privatekey= rsa.ToXmlString(true);
        Eloi.E_FileAndFolderUtility.ExportByOverriding(m_nearFile, privatekey);
        LoadPrivateKeyNearIfThere();

    }
}
