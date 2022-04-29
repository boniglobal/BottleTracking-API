#pragma warning disable SYSLIB0021 // Type or member is obsolete
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
namespace Core.Utilities.Encryption
{
    public class AesEncryptor : IAesEncryptor
    {
        private readonly EncryptionSettings _encryptionSettings;
        private readonly byte[] buffer = new byte[] { 0x00, 0x00, 0x00 };
        public AesEncryptor(IOptions<EncryptionSettings> encryptionSettings)
        {
            _encryptionSettings = encryptionSettings.Value;
        }

        class AesCounterMode : SymmetricAlgorithm
        {
            private readonly ulong _nonce;
            private readonly ulong _counter;
            private readonly AesManaged _aes;
            public AesCounterMode(byte[] nonce, ulong counter)
              : this(ConvertNonce(nonce), counter)
            {
            }

            public AesCounterMode(ulong nonce, ulong counter)
            {
                _aes = new AesManaged
                {
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.None
                };

                _nonce = nonce;
                _counter = counter;
            }

            private static ulong ConvertNonce(byte[] nonce)
            {
                if (nonce == null) throw new ArgumentNullException(nameof(nonce));
                if (nonce.Length < sizeof(ulong)) throw new ArgumentException($"{nameof(nonce)} must have at least {sizeof(ulong)} bytes");

                return BitConverter.ToUInt64(nonce);
            }

            public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] ignoredParameter)
            {
                return new CounterModeCryptoTransform(_aes, rgbKey, _nonce, _counter);
            }

            public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] ignoredParameter)
            {
                return new CounterModeCryptoTransform(_aes, rgbKey, _nonce, _counter);
            }

            public override void GenerateKey()
            {
                _aes.GenerateKey();
            }

            public override void GenerateIV()
            {
                // IV not needed in Counter Mode
            }
        }

        class CounterModeCryptoTransform : ICryptoTransform
        {
            private readonly byte[] _nonceAndCounter;
            private readonly ICryptoTransform _counterEncryptor;
            private readonly Queue<byte> _xorMask = new();
            private readonly SymmetricAlgorithm _symmetricAlgorithm;

            private ulong _counter;

            public CounterModeCryptoTransform(SymmetricAlgorithm symmetricAlgorithm, byte[] key, ulong nonce, ulong counter)
            {
                if (key == null) throw new ArgumentNullException(nameof(key));

                _symmetricAlgorithm = symmetricAlgorithm ?? throw new ArgumentNullException(nameof(symmetricAlgorithm));
                _counter = counter;
                _nonceAndCounter = new byte[16];
                BitConverter.TryWriteBytes(_nonceAndCounter, nonce);
                BitConverter.TryWriteBytes(new Span<byte>(_nonceAndCounter, sizeof(ulong), sizeof(ulong)), counter);

                var zeroIv = new byte[_symmetricAlgorithm.BlockSize / 8];
                _counterEncryptor = symmetricAlgorithm.CreateEncryptor(key, zeroIv);
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                var output = new byte[inputCount];
                TransformBlock(inputBuffer, inputOffset, inputCount, output, 0);
                return output;
            }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer,
                int outputOffset)
            {
                for (var i = 0; i < inputCount; i++)
                {
                    if (NeedMoreXorMaskBytes())
                    {
                        EncryptCounterThenIncrement();
                    }

                    var mask = _xorMask.Dequeue();
                    outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ mask);
                }

                return inputCount;
            }

            private bool NeedMoreXorMaskBytes()
            {
                return _xorMask.Count == 0;
            }

            private byte[] _counterModeBlock;
            private void EncryptCounterThenIncrement()
            {
                _counterModeBlock ??= new byte[_symmetricAlgorithm.BlockSize / 8];

                _counterEncryptor.TransformBlock(_nonceAndCounter, 0, _nonceAndCounter.Length, _counterModeBlock, 0);
                IncrementCounter();

                foreach (var b in _counterModeBlock)
                {
                    _xorMask.Enqueue(b);
                }
            }

            private void IncrementCounter()
            {
                _counter++;
                var span = new Span<byte>(_nonceAndCounter, sizeof(ulong), sizeof(ulong));
                BitConverter.TryWriteBytes(span, _counter);
            }

            public int InputBlockSize => _symmetricAlgorithm.BlockSize / 8;
            public int OutputBlockSize => _symmetricAlgorithm.BlockSize / 8;
            public bool CanTransformMultipleBlocks => true;
            public bool CanReuseTransform => false;

            public void Dispose()
            {
                _counterEncryptor.Dispose();
            }
        }

        static byte[] TrimEnd(byte[] array)
        {
            var newArray = array.Take(5).ToArray();
            return newArray;
        }

        public long EncryptData(long id)
        {
            var data = TrimEnd(BitConverter.GetBytes(id));

            Array.Reverse(data);

            byte[] key = Convert.FromHexString(_encryptionSettings.Key);
            byte[] nonce = Convert.FromHexString(_encryptionSettings.Nonce);
            ulong counter = (ulong)_encryptionSettings.Counter;
            
            using var counterMode = new AesCounterMode(nonce, counter);
            using var encryptor = counterMode.CreateEncryptor(key, null);

            var encryptedData = new byte[data.Length];

            var bytesWritten = encryptor.TransformBlock(data, 0, data.Length, encryptedData, 0);
            var encryptedTrackingId = buffer.Concat(encryptedData).ToArray();
            Array.Reverse(encryptedTrackingId);

            return BitConverter.ToInt64(encryptedTrackingId);
        }

        public long DecryptData(long trackingId)
        {
            byte[] key = Convert.FromHexString(_encryptionSettings.Key);
            byte[] nonce = Convert.FromHexString(_encryptionSettings.Nonce);
            ulong counter = (ulong)_encryptionSettings.Counter;

            using var counterMode = new AesCounterMode(nonce, counter);
            using var decryptor = counterMode.CreateDecryptor(key, null);
            
            var encData = TrimEnd(BitConverter.GetBytes(trackingId));
            Array.Reverse(encData);
            decryptor.TransformBlock(encData, 0, encData.Length, encData, 0);
            var decryptedData = buffer.Concat(encData).ToArray();
            Array.Reverse(decryptedData);

            return BitConverter.ToInt64(decryptedData);
        }
    }

    public class EncryptionSettings
    {
        public string Key { get; set; }
        public string Nonce { get; set; }
        public long Counter { get; set; }
    }
}
#pragma warning restore SYSLIB0021 // Type or member is obsolete

