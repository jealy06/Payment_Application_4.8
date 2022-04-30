import sys
import clr
assemblyPath = r"C:\Users\jarre\OneDrive\Desktop\Csharp\Payment_Application\bin\Debug\net48"
sys.path.append(assemblyPath)

print(clr.FindAssembly("Payment_Application"))
#print(clr.AddReference("System.Windows.Forms"))
clr.AddReference('Payment_Application')
from Payment_Application import Program
obj = Program()
print(obj.OutOfScope())
print(obj.sendList())
