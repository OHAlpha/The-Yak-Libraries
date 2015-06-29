using System;

namespace OAlphaCollections
{
    public interface ElementConverter<I,O>
    {

        O Convert(I e);

    }

}