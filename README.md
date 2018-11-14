# MementoFX
An application framework supporting the development of time travelling applications

**Release notes**
Version 2.0.0-pre6
- IEventStore.Find(...) signature changed to allow for an easier implementation of RDBMs' support

Version 2.0.0-pre5
- Support for .NET Standard 2
- Unit tests migrated to XUnit
- IRepository.SaveAsync introduced to support async/await
- Strong name key file removed
- Timestamps are now UTC-based