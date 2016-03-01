# Ohana3DS Transfigured
A modification of the popular program Ohana3DS Rebirth, for extracting data from Nintendo 3DS ROM files.

This program is my attempt to add features and correct some issues with Ohana3DS Rebirth to fit my own needs, published here on Github to better organize the code and share these new features with my peers.

Differences between Transfigured and Rebirth include:
The ability to read .pb and .pk animation files included in ROM dumps for Pokémon X and Y, and Pokémon Omega Ruby and Alpha Sapphire.
Corrections to the code which exports 3D models and animations to Autodesk COLLADA (.dae) files, which at this point is still not complete at Rebirth.
Batch processing, in the form of multiple-file drag-drop support. Because of technical issues, this is currently actually "drag-enter" processing: The program batch-reads and exports files as soon as they are dragged into the window, not when the mouse button is released.

Planned features include support for exporting animations other than skeletal movement, which are a bit difficult to encode in other formats.

Ohana3DS Rebirth is a reboot of the program Ohana3DS, reconstructed in Visual C# based on the original, which was written in Visual Basic. This program uses large amounts of code drawn from Ohana3DS Rebirth, originally written by GDKChan of the VG Resource. This program is not meant to be individual: Periodically, this program will undergo "version shifts," a term I have given to the process of updating the original code based on Rebirth to the latest version published by GDKChan, which in most cases will likely mandate rewriting large segments of my modifications.
